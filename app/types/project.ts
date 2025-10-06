import * as zod from 'zod'

import { ProjectColumnSchema } from './project-column'

export const ProjectSchema = zod.object({
  clientId: zod.string()
    .optional(),
  id: zod.number()
    .int()
    .nonnegative()
    .optional(),
  title: zod.string()
    .min(3, { message: 'This is required' })
    .max(200, { error: 'Too long (max: 200)' }),
  description: zod.string()
    .optional(),
  projectColumns: zod.array(ProjectColumnSchema)
})

export type Project = zod.infer<typeof ProjectSchema>

export const DefaultProjectState: Project = {
  title: 'New Project',
  description: '',
  projectColumns: []
}