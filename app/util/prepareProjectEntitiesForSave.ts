import type { Project, ProjectColumn, WorkItem } from '~/types';

export function prepareProjectEntityForSave(entity: Project): Project {
  return {
    ...entity,
    clientId: undefined,
    projectColumns: prepareColumnEntitiesForSave(entity.projectColumns)
  }
}

export function prepareProjectEntitiesForSave(entities: Project[]): Project[] {
  return (entities || []).map(prepareProjectEntityForSave);
}

export function prepareColumnEntitiesForSave(entities: ProjectColumn[] | undefined): ProjectColumn[] {
  return (entities || []).map(entity => ({
    ...entity,
    clientId: undefined,
    workItems: prepareWorkItemEntitiesForSave(entity.workItems)
  }))
}

export function prepareWorkItemEntitiesForSave(entities: WorkItem[] | undefined): WorkItem[] {
  return (entities || []).map(entity => ({
    ...entity,
    clientId: undefined
  }))
}