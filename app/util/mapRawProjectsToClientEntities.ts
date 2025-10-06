import type { Project, ProjectColumn, WorkItem } from '~/types';

export function mapRawProjectToClientEntity(entity: Project): Project {
  return {
    ...entity,
    clientId: crypto.randomUUID(),
    projectColumns: mapRawColumnsToClientEntities(entity.projectColumns)
  }
}

export function mapRawColumnToClientEntity(entity: ProjectColumn): ProjectColumn {
  return {
    ...entity,
    clientId: crypto.randomUUID(),
    workItems: mapRawWorkItemsToClientEntities(entity.workItems)
  }
}

export function mapRawColumnsToClientEntities(entities: ProjectColumn[] | undefined): ProjectColumn[] {
  return (entities || []).map(mapRawColumnToClientEntity)
}

export function mapRawWorkItemToClientEntity(entity: WorkItem) {
  return {
    ...entity,
    clientId: crypto.randomUUID()
  }
}

export function mapRawWorkItemsToClientEntities(entities: WorkItem[] | undefined): WorkItem[] {
  return (entities || []).map(mapRawWorkItemToClientEntity)
}
