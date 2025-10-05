import type { Project, ProjectColumn, WorkItem } from '~/types';

export function mapRawProjectToClientEntity(previousEntities?: Project[]) {
  return (entity: Project): Project => {
    let previousEntity: Project | undefined;

    if (previousEntities?.length) {
      previousEntity = previousEntities.find(x => x.id === entity.id)
    }

    return {
      ...entity,
      clientId: previousEntity?.clientId || crypto.randomUUID(),
      columns: mapRawColumnsToClientEntities(entity.columns, previousEntity?.columns)
    }
  }
}

export function mapRawProjectsToClientEntities(entities: Project[], previousEntities?: Project[]): Project[] {
  return (entities || []).map(mapRawProjectToClientEntity(previousEntities))
}

export function mapRawColumnToClientEntity(previousEntities?: ProjectColumn[]) {
  return (entity: ProjectColumn): ProjectColumn =>{
    let previousEntity: ProjectColumn | undefined;

    if (previousEntities?.length) {
      previousEntity = previousEntities.find(x => x.id === entity.id)
    }

    return {
      ...entity,
      clientId: previousEntity?.clientId || crypto.randomUUID(),
      workItems: mapRawWorkItemsToClientEntities(entity.workItems, previousEntity?.workItems)
    }
  }
}

export function mapRawColumnsToClientEntities(entities: ProjectColumn[] | undefined, previousEntities?: ProjectColumn[]): ProjectColumn[] {
  return (entities || []).map(mapRawColumnToClientEntity(previousEntities))
}

export function mapRawWorkItemToClientEntity(previousEntities?: WorkItem[]) {
  return (entity: WorkItem) => {
    let previousEntity: WorkItem | undefined;

    if (previousEntities?.length) {
      previousEntity = previousEntities.find(x => x.id === entity.id)
    }

    return {
      ...entity,
      clientId: previousEntity?.clientId || crypto.randomUUID()
    }
  }
}

export function mapRawWorkItemsToClientEntities(entities: WorkItem[] | undefined, previousEntities?: WorkItem[]): WorkItem[] {
  return (entities || []).map(mapRawWorkItemToClientEntity(previousEntities))
}
