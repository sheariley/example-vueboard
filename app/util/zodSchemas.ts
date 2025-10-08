import * as zod from 'zod'

export const hexColor = zod.string().regex(
  /^#([A-Fa-f0-9]{6}|[A-Fa-f0-9]{3})$/,
  { 
    message: 'Invalid hex color format. Must be a 3 or 6 digit hex code starting with #.'
  }
)