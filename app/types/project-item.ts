import * as zod from 'zod'

export const ProjectItemSchema = zod.object({
  clientId: zod.string().optional(),
  projectColumnId: zod.number().int().nonnegative().optional(),
  id: zod.number().int().nonnegative().optional(),
  title: zod.string().min(3, { message: 'This is required' }),
  content: zod.string().optional(),
  tags: zod.array(zod.string().min(1, { message: 'A tag must not be empty' }))
})

export type ProjectItem = zod.infer<typeof ProjectItemSchema>
