export type DraggableChangedEvent<TElement = unknown> = {
  added?: {
    newIndex: number
    element: TElement
  }

  removed?: {
    oldIndex: number
    element: TElement
  }

  moved?: {
    newIndex: number
    oldIndex: number
    element: TElement
  }
}
