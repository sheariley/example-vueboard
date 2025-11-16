import { TextSelection, AllSelection } from '@milkdown/kit/prose/state';
import { $command } from '@milkdown/utils';
import { findNodeInSelection } from '@milkdown/prose';
import type { Node, NodeType, ResolvedPos } from '@milkdown/prose/model';
import { listItemSchema } from '@milkdown/kit/preset/commonmark';

export const clearAllMarksCommand = $command('ClearAllMarks', () => () => (state, dispatch) => {
  const { tr, selection } = state;
  const { from, to } = selection;

  if (selection instanceof TextSelection || selection instanceof AllSelection) {
    tr.removeMark(from, to);
    dispatch?.(tr);
    return true;
  }

  return false;
})

export const getNodeAttributeCommand = $command('GetNodeAttribute', (ctx) => (payload?: GetNodeAttributePayload) => (state) => {
  if (!payload) {
    throw new Error('The payload parameter is required for GetNodeAttribute command.');
  }

  const { attr, nodeType, callback } = payload
  const { selection } = state

  // Check if a selection exists
  if (!selection) {
    return false
  }

  const resolvedPos = state.doc.resolve(selection.from);
  const findNodeResult = findNodeInSelection(state, nodeType)
  if (!findNodeResult.hasNode) {
    callback(undefined)
    return false;
  }

  const matchingNode = findNodeAncestor(resolvedPos, node => 
    node.type === nodeType && typeof node.attrs[attr] !== 'undefined'
  )
  if (matchingNode) {
    callback(matchingNode.attrs[attr])
    return true
  }

  return false
});

export const isSelectionInNodeCommand = $command('IsSelectionInNode', (ctx) => (payload?: IsSelectionInNodePayload) => (state) => {
  if (!payload) {
    throw new Error('The payload parameter is required for IsSelectionInNode command.');
  }

  const { matcher, nodeType } = payload
  const { selection } = state

  if (!selection) {
    return false
  }

  const resolvedPos = state.doc.resolve(selection.from);
  const findNodeResult = findNodeInSelection(state, nodeType)
  if (!findNodeResult.hasNode) {
    return false
  }
  
  const matchingNode = findNodeAncestor(resolvedPos, node => 
    node.type === nodeType && matcher(node)
  )

  return !!matchingNode
})

function findNodeAncestor(pos: ResolvedPos, predicate: (node: Node) => boolean) {
  if (predicate(pos.parent)) return pos.parent

  if (pos.depth < 1) return null
  
    // search down thru lineage
  for (let d = pos.depth - 1; d >= 0; d--) {
    const ancestor = pos.node(d)
    if (!!ancestor && predicate(ancestor)) return ancestor
  }

  return null
}

export type GetNodeAttributePayload = {
  attr: string
  nodeType: NodeType
  callback(result: any): void
}

export type HasNodeAttributePayload = {
  attr: string
  nodeType: NodeType
}

export type IsSelectionInNodePayload = {
  matcher(node: Node): boolean
  nodeType: NodeType
}
