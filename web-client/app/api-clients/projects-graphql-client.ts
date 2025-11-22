import { ApolloClient, ApolloLink, gql, HttpLink, InMemoryCache } from '@apollo/client/core';
import { SetContextLink } from '@apollo/client/link/context';
import { RemoveTypenameFromVariablesLink } from '@apollo/client/link/remove-typename';

import type { Project } from '~/types';
import type { ProjectListItem } from '~/types/project-list-item';
import type { WorkItemTag } from '~/types/work-item-tag';

// TypeScript types for GraphQL responses
interface ProjectResponse {
  project: Project | null
}

interface UpdateProjectResponse {
  updateProject: Project
}

interface DeleteProjectResponse {
  deleteProject: boolean
}

export function useProjectsGraphQLClient() {
  const config = useRuntimeConfig()
  const endpoint = config.public.projectsGraphqlBase || '/graphql'
  const authStore = useAuthStore()

  // use SetContextLink to forward JWT access token
  const authLink = new SetContextLink(async ({ headers }) => {
    const token = authStore.accessToken
    return {
      headers: {
        ...headers,
        Authorization: token ? `Bearer ${token}` : '',
      },
    }
  })

  const removeTypenameLink = new RemoveTypenameFromVariablesLink();

  const httpLink = new HttpLink({
    uri: endpoint,
    credentials: 'include',
  })

  const client = new ApolloClient({
    link: ApolloLink.from([authLink, removeTypenameLink, httpLink]),
    cache: new InMemoryCache(),
  })

  async function fetchAllProjects(): Promise<Project[]> {
    const query = gql`
      query GetAllProjects {
        projects {
          id
          uid
          created
          updated
          isDeleted
          title
          description
          defaultCardFgColor
          defaultCardBgColor
          columns {
            id
            uid
            name
            index
            fgColor
            bgColor
            projectId
            workItems {
              id
              uid
              title
              index
              description
              notes
              tags
              fgColor
              bgColor
              projectColumnId,
              workItemTags {
                id
                uid
                tagText
              }
            }
          }
        }
      }
    `
    const result = await client.query<{ projects: Project[] }>({
      query,
      fetchPolicy: 'network-only',
    })
    if (!result.data?.projects) throw new Error('No projects found')
    return result.data.projects
  }

  async function fetchProject(projectUid: string): Promise<Project> {
    const query = gql`
      query GetProject($uid: UUID!) {
        project(uid: $uid) {
          id
          uid
          created
          updated
          isDeleted
          title
          description
          defaultCardFgColor
          defaultCardBgColor
          projectColumns {
            id
            uid
            name
            index
            isDefault
            fgColor
            bgColor
            projectId
            workItems {
              id
              uid
              title
              index
              description
              notes
              fgColor
              bgColor
              projectColumnId
              workItemTags {
                id
                uid
                tagText
              }
            }
          }
        }
      }
    `
    const result = await client.query<ProjectResponse>({
      query,
      variables: { uid: projectUid },
      fetchPolicy: 'network-only',
    })
    if (!result.data?.project) throw new Error(`Project not found (uid: ${projectUid})`)
    return result.data.project as Project
  }

  async function fetchProjectListItems(): Promise<ProjectListItem[]> {
    const query = gql`
      query GetProjectListItems {
        projects {
          uid
          id
          title
          description
        }
      }
    `
    const result = await client.query<{ projects: ProjectListItem[] }>({
      query,
      fetchPolicy: 'network-only',
    })
    if (!result.data?.projects) throw new Error('No project list items found')
    return result.data.projects
  }

  async function createProject(input: {
    uid?: string
    title: string
    description?: string
    defaultCardFgColor?: string
    defaultCardBgColor?: string
  }): Promise<Project> {
    const mutation = gql`
      mutation CreateProject($input: CreateProjectInput!) {
        createProject(input: $input) {
          uid
          title
          description
          defaultCardFgColor
          defaultCardBgColor
        }
      }
    `
    const result = await client.mutate<{ createProject: Project }>({
      mutation,
      variables: { input },
    })
    if (!result.data?.createProject) throw new Error('Error creating project')
    return result.data.createProject
  }

  async function saveProject(input: Project): Promise<Project> {
    const mutation = gql`
      mutation UpdateProject($input: UpdateProjectInput!) {
        updateProject(input: $input) {
          id
          uid
          updated
          isDeleted
          title
          description
          defaultCardFgColor
          defaultCardBgColor
          projectColumns {
            id
            uid
            projectId
            name
            index
            isDefault
            fgColor
            bgColor
            projectId
            workItems {
              id
              uid
              title
              index
              description
              notes
              fgColor
              bgColor
              projectColumnId
              workItemTags {
                id
                uid
                tagText
              }
            }
          }
        }
      }
    `
    const result = await client.mutate<UpdateProjectResponse>({
      mutation,
      variables: { input },
    })
    if (!result.data?.updateProject) throw new Error('Error saving project')
    return result.data.updateProject as Project
  }

  async function deleteProject(projectUid: string): Promise<boolean> {
    const mutation = gql`
      mutation DeleteProject($uid: UUID!) {
        deleteProject(uid: $uid)
      }
    `
    const result = await client.mutate<DeleteProjectResponse>({
      mutation,
      variables: { uid: projectUid },
    })
    if (!result.data) throw new Error('Error deleting project')
    return result.data.deleteProject === true
  }

  async function fetchAllWorkItemTags(): Promise<WorkItemTag[]> {
    const query = gql`
      query GetAllWorkItemTags {
        workItemTags {
          id
          uid
          tagText
        }
      }
    `
    const result = await client.query<{ workItemTags: WorkItemTag[] }>({
      query,
      fetchPolicy: 'network-only',
    })
    if (!result.data?.workItemTags) throw new Error('No WorkItemTags found')
    return result.data.workItemTags
  }

  return {
    fetchProject,
    fetchAllProjects,
    fetchProjectListItems,
    saveProject,
    deleteProject,
    createProject,
    fetchAllWorkItemTags,
  }
}
