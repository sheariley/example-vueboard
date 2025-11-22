import * as zod from 'zod'
import { ProjectOptionsSchema } from './project-options'

export const ProjectListItemSchema = ProjectOptionsSchema.extend({
  uid: zod.uuidv4(),
  id: zod.number()
    .int()
    .nonnegative()
    .optional(),
  userId: zod.string()
    .optional(),
})

export type ProjectListItem = zod.infer<typeof ProjectListItemSchema>
