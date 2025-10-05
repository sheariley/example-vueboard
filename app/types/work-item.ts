import * as zod from 'zod'

export const WorkItemSchema = zod.object({
  clientId: zod.string()
    .optional(),
  projectColumnId: zod.number()
    .int()
    .nonnegative()
    .optional(),
  id: zod.number()
    .int()
    .nonnegative()
    .optional(),
  title: zod.string()
    .min(3, { message: 'This is required' })
    .max(200, { error: 'Too long (max: 200)' }),
  content: zod.string()
    .optional(),
  tags: zod.array(zod.string()
    .min(1, { message: 'A tag must not be empty' })
    .max(30, { error: 'Too long (max: 30)' })
  )
})

export type WorkItem = zod.infer<typeof WorkItemSchema>
