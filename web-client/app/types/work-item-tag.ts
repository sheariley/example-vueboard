import * as zod from 'zod'

export const WorkItemTagSchema = zod.object({
  uid: zod.uuidv4(),
  id: zod.number()
    .int()
    .nonnegative()
    .optional(),
  userId: zod.string()
    .optional(),
  tagText: zod.string()
    .min(1, 'This is required')
    .max(30, 'Too long (max: 30)')
})

export type WorkItemTag = zod.infer<typeof WorkItemTagSchema>

export const DefaultWorkItemTagState: WorkItemTag = {
  uid: '',
  tagText: ''
}
