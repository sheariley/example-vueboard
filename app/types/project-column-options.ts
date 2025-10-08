import * as zod from 'zod'
import { hexColor } from '~/util/zodSchemas'


// TODO: Add more restrictive validation on name property (only allow alphanumeric and limited symbols such as dash and underscore)
export const ProjectColumnOptionsSchema = zod.object({
  name: zod.string()
    .min(3, { message: 'This is required and must be at least 3 characters long.' }),
  fgColor: hexColor
    .optional(),
  bgColor: hexColor
    .optional()
})

export type ProjectColumnOptions = zod.infer<typeof ProjectColumnOptionsSchema>
