import { ApolloClient, InMemoryCache, gql, HttpLink } from '@apollo/client/core'
import { SetContextLink } from '@apollo/client/link/context'
import { useRuntimeConfig } from '#imports'
import type { Project } from '~/types'
import type { ProjectListItem } from '~/types/project-list-item'

// TypeScript types for GraphQL responses
interface ProjectColumn {
  id: number;
  uid: string;
  name: string;
  index: number;
  fgColor?: string;
  bgColor?: string;
  projectId?: number;
  workItems?: Array<{
    id: number;
    uid: string;
    title: string;
    index: number;
    description?: string;
    notes?: string;
    tags?: string[];
    fgColor?: string;
    bgColor?: string;
    projectColumnId?: number;
  }>;
}

interface ProjectResponse {
  project: {
    id: number;
    uid: string;
    created: string;
    updated: string;
    isDeleted: boolean;
    userId: string;
    title: string;
    description?: string;
    defaultCardFgColor?: string;
    defaultCardBgColor?: string;
    projectColumns: ProjectColumn[];
  } | null;
}

interface UpdateProjectResponse {
  updateProject: Project;
}

interface DeleteProjectResponse {
  deleteProject: boolean;
}

export function useProjectsGraphQLClient() {
  const config = useRuntimeConfig();
  const endpoint = config.public.projectsGraphqlBase || '/graphql';
  const supabase = useSupabaseClient()

  // use SetContextLink to forward JWT access token
  const authLink = new SetContextLink(async ({ headers }) => {
    const getSessionResult = await supabase.auth.getSession()
    if (getSessionResult.error) {
      throw new Error('Error obtaining auth session', {
        cause: getSessionResult.error
      })
    }

    const token = getSessionResult.data.session?.access_token
    return {
      headers: {
        ...headers,
        Authorization: token ? `Bearer ${token}` : '',
      },
    };
  });

  const httpLink = new HttpLink({
    uri: endpoint,
    credentials: 'include',
  });

  const client = new ApolloClient({
    link: authLink.concat(httpLink),
    cache: new InMemoryCache(),
  });

  async function fetchAllProjects(): Promise<Project[]> {
    const query = gql`
      query GetAllProjects {
        projects {
          id
          uid
          created
          updated
          isDeleted
          userId
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
              projectColumnId
            }
          }
        }
      }
    `;
    const result = await client.query<{ projects: Project[] }>({
      query,
      fetchPolicy: 'network-only',
    });
    if (!result.data?.projects) throw new Error('No projects found');
    return result.data.projects;
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
          userId
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
              projectColumnId
            }
          }
        }
      }
    `;
    const result = await client.query<ProjectResponse>({
      query,
      variables: { uid: projectUid },
      fetchPolicy: 'network-only',
    });
    if (!result.data?.project) throw new Error(`Project not found (uid: ${projectUid})`);
    // No mapping needed, projectColumns is now returned directly
    return result.data.project as Project;
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
    `;
    const result = await client.query<{ projects: ProjectListItem[] }>({
      query,
      fetchPolicy: 'network-only',
    });
    if (!result.data?.projects) throw new Error('No project list items found');
    return result.data.projects;
  }

  async function saveProject(project: Project): Promise<Project> {
    const mutation = gql`
      mutation SaveProject($input: ProjectInput!) {
        updateProject(project: $input) {
          id
          uid
          title
        }
      }
    `;
    const result = await client.mutate<UpdateProjectResponse>({
      mutation,
      variables: { input: project },
    });
    if (!result.data?.updateProject) throw new Error('Error saving project');
    return result.data.updateProject as Project;
  }

  async function deleteProject(projectUid: string): Promise<boolean> {
    const mutation = gql`
      mutation DeleteProject($uid: UUID!) {
        deleteProject(uid: $uid)
      }
    `;
    const result = await client.mutate<DeleteProjectResponse>({
      mutation,
      variables: { uid: projectUid },
    });
    if (!result.data) throw new Error('Error deleting project');
    return result.data.deleteProject === true;
  }

  return {
    fetchProject,
    fetchAllProjects,
    fetchProjectListItems,
    saveProject,
    deleteProject,
  };
}
