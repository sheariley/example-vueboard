import type { FontAwesomeIconProps } from '@fortawesome/vue-fontawesome';
import type { AllowedComponentProps } from 'vue';

export function createFontAwesomeIcon(props: string | (FontAwesomeIconProps & AllowedComponentProps)) {
  if (typeof props === 'string') {
    props = { icon: props }
  }
  return h(FontAwesomeIcon, props)
}