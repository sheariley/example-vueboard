import * as zod from 'zod'
import { hexColor } from '~/util/zodSchemas'

export const ProjectOptionsSchema = zod.object({
  title: zod.string()
    .min(3, { message: 'This is required' })
    .max(200, { error: 'Too long (max: 200)' }),
  description: zod.string()
    .max(600, { error: 'Too long (max: 600)' })
    .optional(),
  defaultCardFgColor: hexColor
    .optional(),
  defaultCardBgColor: hexColor
    .optional()
})

export type ProjectOptions = zod.infer<typeof ProjectOptionsSchema>

export const DefaultProjectOptionsState: ProjectOptions = {
  title: 'New Project',
  description: '',
  defaultCardBgColor: '#005C36',
  defaultCardFgColor: '#FFFFFF'
}
