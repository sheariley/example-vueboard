import * as zod from 'zod'

import { WorkItemSchema } from './work-item'

export const ProjectColumnSchema = zod.object({
  uid: zod.uuidv4(),
  projectId: zod.number()
    .int()
    .nonnegative()
    .optional(),
  id: zod.number()
    .int()
    .nonnegative()
    .optional(),
  name: zod.string()
    .min(3, { message: 'This is required' }),
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
