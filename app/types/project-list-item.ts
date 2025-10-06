import * as zod from 'zod'
import { ProjectInfoSchema } from './project-info'

export const ProjectListItemSchema = ProjectInfoSchema.extend({
  uid: zod.uuidv4(),
  id: zod.number()
    .int()
    .nonnegative()
    .optional()
})

export type ProjectListItem = zod.infer<typeof ProjectListItemSchema>
