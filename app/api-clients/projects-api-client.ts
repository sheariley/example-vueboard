import type { RuntimeConfig } from 'nuxt/schema';
import type { Project } from '~/types';

export default function createClient(config: RuntimeConfig) {
  
  async function fetchProject(projectUid: string) {
    const url = `${config.public.projectsApiBase}/projectByUid/${projectUid}`;

    const resp = await fetch(url);
    if (!resp.ok) {
      throw new Error(`Error fetching project: ${resp.statusText} (${resp.status})`, { cause: resp });
    }

    const results: Project[] = await resp.json();

    if (!results?.length) {
      throw new Error(`Project not found (uid: ${projectUid})`)
    }

    return results[0]!;    
  }

  async function saveProject(project: Project) {
    let method = 'POST';
    let url = `${config.public.projectsApiBase}/projects`;

    if (project?.id) {
      url += `/${project.id}`;
      method = 'PUT';
    }

    const body = JSON.stringify(project);
    const resp = await fetch(url, {
      headers: {
        'Content-Type': 'application/json',
      },
      method,
      body,
    });

    if (!resp.ok) {
      throw new Error(`Error saving project: ${resp.statusText} (${resp.status})`, { cause: resp });
    }

    const savedEntity: Project = await resp.json();

    return savedEntity;
  }

  async function deleteProject(projectUid: string) {
    const url = `${config.public.projectsApiBase}/projectByUid/${projectUid}`;

    const resp = await fetch(url, {
      method: 'DELETE'
    })

    if (!resp.ok) {
      throw new Error(`Error saving project: ${resp.statusText} (${resp.status})`, { cause: resp });
    }

    return true
  }

  return {
    fetchProject,
    saveProject,
    deleteProject
  }
}

