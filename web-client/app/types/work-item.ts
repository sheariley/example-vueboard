import * as zod from 'zod'

import { WorkItemOptionsSchema } from './work-item-options'

export const WorkItemSchema = WorkItemOptionsSchema.extend({
  uid: zod.uuidv4(),
  projectColumnId: zod.number()
    .int()
    .nonnegative(),
  id: zod.number()
    .int()
    .nonnegative(),
  index: zod.number()
    .int()
    .nonnegative(),
})

export type WorkItem = zod.infer<typeof WorkItemSchema>

export const DefaultWorkItemState: WorkItem = {
  id: 0,
  uid: '',
  projectColumnId: 0,
  title: '',
  description: null,
  notes: null,
  workItemTags: [],
  fgColor: undefined,
  bgColor: undefined,
  index: 0
}
