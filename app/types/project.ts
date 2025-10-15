import * as zod from 'zod'

import { ProjectColumnSchema } from './project-column'
import { ProjectListItemSchema } from './project-list-item'
import { DefaultProjectOptionsState } from './project-options'

export const ProjectSchema = ProjectListItemSchema.extend({
  projectColumns: zod.array(ProjectColumnSchema)
})

export type Project = zod.infer<typeof ProjectSchema>

export const DefaultProjectState: Project = {
  ...DefaultProjectOptionsState,
  uid: '',
  projectColumns: []
}
