import * as zod from 'zod'
import { hexColor } from '~/util/zodSchemas'

export const WorkItemOptionsSchema = zod.object({
  title: zod.string()
    .min(3, { message: 'This is required' })
    .max(200, { error: 'Too long (max: 200)' }),
  description: zod.string()
    .nullable(),
  notes: zod.string()
    .nullable(),
  tags: zod.array(zod.string()
    .min(1, { message: 'A tag must not be empty' })
    .max(30, { error: 'Too long (max: 30)' })
  ),
  fgColor: hexColor
    .optional(),
  bgColor: hexColor
    .optional()
})

export type WorkItemOptions = zod.infer<typeof WorkItemOptionsSchema>
