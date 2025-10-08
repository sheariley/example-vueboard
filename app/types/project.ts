import * as zod from 'zod'

import { ProjectColumnSchema } from './project-column'
import { ProjectListItemSchema } from './project-list-item'

export const ProjectSchema = ProjectListItemSchema.extend({
  projectColumns: zod.array(ProjectColumnSchema)
})

export type Project = zod.infer<typeof ProjectSchema>

export const DefaultProjectState: Project = {
  uid: '',
  title: 'New Project',
  description: '',
  defaultCardBgColor: '#005C36',
  defaultCardFgColor: '#FFFFFF',
  projectColumns: []
}