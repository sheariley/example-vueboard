import * as zod from 'zod'

export const WorkItemSchema = zod.object({
  uid: zod.uuidv4(),
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
    .nullable()
    .optional(),
  tags: zod.array(zod.string()
    .min(1, { message: 'A tag must not be empty' })
    .max(30, { error: 'Too long (max: 30)' })
  ),
  index: zod.number().int().nonnegative()
})

export type WorkItem = zod.infer<typeof WorkItemSchema>

export const DefaultWorkItemState: WorkItem = {
  uid: '',
  title: '',
  tags: [],
  index: 0
}
