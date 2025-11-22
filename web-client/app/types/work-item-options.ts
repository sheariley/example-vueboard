import * as zod from 'zod'
import { hexColor } from '~/util/zodSchemas'
import { WorkItemTagSchema } from './work-item-tag'

export const WorkItemOptionsSchema = zod.object({
  title: zod.string()
    .min(3, { message: 'This is required' })
    .max(200, { error: 'Too long (max: 200)' }),
  description: zod.string()
    .max(600, { error: 'Too long (max: 600)' })
    .nullable(),
  notes: zod.string()
    .nullable(),
  workItemTags: zod.array(WorkItemTagSchema)
    .optional(),
  fgColor: hexColor
    .optional(),
  bgColor: hexColor
    .optional()
})

export type WorkItemOptions = zod.infer<typeof WorkItemOptionsSchema>
