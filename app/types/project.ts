import * as zod from 'zod'

import { ProjectColumnSchema } from './project-column'

export const ProjectSchema = zod.object({
  clientId: zod.string().optional(),
  id: zod.number().int().nonnegative().optional(),
  title: zod.string().min(3, { message: 'This is required' }),
  columns: zod.array(ProjectColumnSchema)
})

export type Project = zod.infer<typeof ProjectSchema>
