import * as zod from 'zod'

export const ProjectInfoSchema = zod.object({
  title: zod.string()
    .min(3, { message: 'This is required' })
    .max(200, { error: 'Too long (max: 200)' }),
  description: zod.string()
    .optional(),
})

export type ProjectInfo = zod.infer<typeof ProjectInfoSchema>
