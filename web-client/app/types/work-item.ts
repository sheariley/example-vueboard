import * as zod from 'zod'
import { hexColor } from '~/util/zodSchemas'

import { WorkItemOptionsSchema } from './work-item-options'

export const WorkItemSchema = WorkItemOptionsSchema.extend({
  uid: zod.uuidv4(),
  projectColumnId: zod.number()
    .int()
    .nonnegative()
    .optional(),
  id: zod.number()
    .int()
    .nonnegative()
    .optional(),
  index: zod.number()
    .int()
    .nonnegative(),
})

export type WorkItem = zod.infer<typeof WorkItemSchema>

export const DefaultWorkItemState: WorkItem = {
  uid: '',
  title: '',
  description: null,
  notes: null,
  tags: [],
  fgColor: undefined,
  bgColor: undefined,
  index: 0
}
