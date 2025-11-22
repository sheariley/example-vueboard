import * as zod from 'zod'

import { WorkItemSchema } from './work-item'
import { ProjectColumnOptionsSchema } from './project-column-options'

export const ProjectColumnSchema = ProjectColumnOptionsSchema.extend({
  uid: zod.uuidv4(),
  projectId: zod.number()
    .int()
    .nonnegative(),
  id: zod.number()
    .int()
    .nonnegative(),
  index: zod.number()
    .int()
    .nonnegative(),
  workItems: zod.array(WorkItemSchema)
    .optional()
})

export type ProjectColumn = zod.infer<typeof ProjectColumnSchema>

export const DefaultProjectColumnState: ProjectColumn = {
  id: 0,
  uid: '',
  projectId: 0,
  index: 0,
  name: 'Backlog',
  isDefault: false,
  workItems: []
}
