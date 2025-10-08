import * as zod from 'zod'

import { WorkItemSchema } from './work-item'
import { ProjectColumnOptionsSchema } from './project-column-options'

export const ProjectColumnSchema = ProjectColumnOptionsSchema.extend({
  uid: zod.uuidv4(),
  projectId: zod.number()
    .int()
    .nonnegative()
    .optional(),
  id: zod.number()
    .int()
    .nonnegative()
    .optional(),
  index: zod.number()
    .int()
    .nonnegative(),
  workItems: zod.array(WorkItemSchema)
    .optional()
})

export type ProjectColumn = zod.infer<typeof ProjectColumnSchema>

export const DefaultProjectColumnState: ProjectColumn = {
  uid: '',
  index: 0,
  name: 'Backlog',
  workItems: []
}
