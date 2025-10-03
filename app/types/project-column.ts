import * as zod from 'zod'

import { ProjectItemSchema } from './project-item'

export const ProjectColumnSchema = zod.object({
  clientId: zod.string().optional(),
  projectId: zod.number().int().nonnegative().optional(),
  id: zod.number().int().nonnegative().optional(),
  name: zod.string().min(3, { message: 'This is required' }),
  index: zod.number().int().nonnegative(),
  items: zod.array(ProjectItemSchema)
})

export type ProjectColumn = zod.infer<typeof ProjectColumnSchema>
