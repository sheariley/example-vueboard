import * as zod from 'zod'

import { WorkItemSchema } from './work-item'

export const ProjectColumnSchema = zod.object({
  clientId: zod.string()
    .optional(),
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
