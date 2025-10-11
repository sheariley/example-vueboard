import { toggleLinkCommand } from '@milkdown/kit/component/link-tooltip';
import { type CommandManager } from '@milkdown/kit/core';
import type { Ctx } from '@milkdown/kit/ctx';
import {
  blockquoteSchema,
  bulletListSchema,
  emphasisSchema,
  headingSchema,
  inlineCodeSchema,
  isMarkSelectedCommand,
  isNodeSelectedCommand,
  linkSchema,
  listItemSchema,
  orderedListSchema,
  paragraphSchema,
  setBlockTypeCommand,
  strongSchema,
  toggleEmphasisCommand,
  toggleInlineCodeCommand,
  toggleStrongCommand,
  wrapInBlockTypeCommand
} from "@milkdown/kit/preset/commonmark";
import { strikethroughSchema, toggleStrikethroughCommand } from '@milkdown/kit/preset/gfm';
import { type $MarkSchema, type $NodeSchema } from '@milkdown/kit/utils';
import { clearAllMarksCommand, isSelectionInNodeCommand } from './custom-editor-commands';

function checkMarkSchemaActive<T extends string = string>(cmdMgr: CommandManager, ctx: Ctx, schema: $MarkSchema<T>) {
  return cmdMgr.call(isMarkSelectedCommand.key, schema.type(ctx))
}

function checkNodeSchemaActive<T extends string = string>(cmdMgr: CommandManager, ctx: Ctx, schema: $NodeSchema<T>) {
  return cmdMgr.call(isNodeSelectedCommand.key, schema.type(ctx))
}

export const MarkdownToolbarCommands: Record<MarkdownToolbarCommandName, MarkdownToolbarCommand> = {
  clear: {
    isActive(cmdMgr: CommandManager, ctx: Ctx): boolean {
      return false; // doesn't need to indicate active state
    },
    run(cmdMgr: CommandManager, ctx: Ctx) {
      const paragraph = paragraphSchema.type(ctx)
      
      cmdMgr.call(clearAllMarksCommand.key);
      cmdMgr.call(setBlockTypeCommand.key, {
        nodeType: paragraph
      })
    }
  },

  bold: {
    isToggle: true,
    isActive(cmdMgr: CommandManager, ctx: Ctx): boolean {
      return checkMarkSchemaActive(cmdMgr, ctx, strongSchema)
    },
    run(cmdMgr: CommandManager) {
      cmdMgr.call(toggleStrongCommand.key)
    }
  },

  italic: {
    isToggle: true,
    isActive(cmdMgr: CommandManager, ctx: Ctx): boolean {
      return checkMarkSchemaActive(cmdMgr, ctx, emphasisSchema)
    },
    run(cmdMgr: CommandManager) {
      cmdMgr.call(toggleEmphasisCommand.key)
    }
  },

  strikethrough: {
    isToggle: true,
    isActive(cmdMgr: CommandManager, ctx: Ctx): boolean {
      return checkMarkSchemaActive(cmdMgr, ctx, strikethroughSchema)
    },
    run(cmdMgr: CommandManager) {
      cmdMgr.call(toggleStrikethroughCommand.key)
    }
  },

  heading: {
    isActive(cmdMgr: CommandManager, ctx: Ctx): boolean {
      return checkNodeSchemaActive(cmdMgr, ctx, headingSchema)
    },
    run(cmdMgr: CommandManager, ctx: Ctx, payload: number) {
      const heading = headingSchema.type(ctx)
      cmdMgr.call(setBlockTypeCommand.key, {
        nodeType: heading,
        attrs: {
          level: payload
        }
      });
    }
  },

  quote: {
    isActive(cmdMgr: CommandManager, ctx: Ctx): boolean {
      return checkNodeSchemaActive(cmdMgr, ctx, blockquoteSchema)
    },
    run(cmdMgr: CommandManager, ctx: Ctx) {
      const quote = blockquoteSchema.type(ctx)
      cmdMgr.call(wrapInBlockTypeCommand.key, {
        nodeType: quote
      })
    }
  },

  link: {
    isToggle: true,
    isActive(cmdMgr: CommandManager, ctx: Ctx): boolean {
      return checkMarkSchemaActive(cmdMgr, ctx, linkSchema)
    },
    run(cmdMgr: CommandManager) {
      cmdMgr.call(toggleLinkCommand.key)
    }
  },

  inline_code: {
    isToggle: true,
    isActive(cmdMgr: CommandManager, ctx: Ctx): boolean {
      return checkMarkSchemaActive(cmdMgr, ctx, inlineCodeSchema)
    },
    run(cmdMgr: CommandManager) {
      cmdMgr.call(toggleInlineCodeCommand.key)
    }
  },

  bullet_list: {
    isActive(cmdMgr: CommandManager, ctx: Ctx): boolean {
      return checkNodeSchemaActive(cmdMgr, ctx, bulletListSchema)
    },
    run(cmdMgr: CommandManager, ctx: Ctx) {
      const bulletList = bulletListSchema.type(ctx)
      cmdMgr.call(wrapInBlockTypeCommand.key, {
        nodeType: bulletList
      });
    }
  },

  ordered_list: {
    isActive(cmdMgr: CommandManager, ctx: Ctx): boolean {
      return checkNodeSchemaActive(cmdMgr, ctx, orderedListSchema)
    },
    run(cmdMgr: CommandManager, ctx: Ctx) {
      const orderedList = orderedListSchema.type(ctx)
      cmdMgr.call(wrapInBlockTypeCommand.key, {
        nodeType: orderedList
      });
    }
  },

  check_list: {
    isActive(cmdMgr: CommandManager, ctx: Ctx): boolean {
      return cmdMgr.call(isSelectionInNodeCommand.key, {
        nodeType: listItemSchema.type(ctx),
        matcher: (node) => typeof node.attrs['checked'] === 'boolean'
      })
    },
    run(cmdMgr: CommandManager, ctx: Ctx) {
      const listItem = listItemSchema.type(ctx)
      cmdMgr.call(wrapInBlockTypeCommand.key, {
        nodeType: listItem,
        attrs: { checked: false }
      });
    }
  }
  
}

export function getActiveToolbarCommandNames(cmdMgr: CommandManager, ctx: Ctx) {
  return getActiveToolbarCommands(cmdMgr, ctx).map(({name}) => name)
}

export function getActiveToolbarCommands(cmdMgr: CommandManager, ctx: Ctx) {
  return Object.entries(MarkdownToolbarCommands)
    .reduce((acc, [name, cmd]) => acc.concat(
      cmd.isActive(cmdMgr, ctx) ? [
        { 
          ...cmd,
          name: name as MarkdownToolbarCommandName
        }
      ] : []
    ), [] as (MarkdownToolbarCommand & { name: MarkdownToolbarCommandName })[])
}

export type MarkdownToolbarCommandName =
  | 'clear'
  | 'bold'
  | 'italic'
  | 'strikethrough'
  | 'heading'
  | 'quote'
  | 'link'
  | 'inline_code'
  | 'bullet_list'
  | 'ordered_list'
  | 'check_list'
  
export type MarkdownToolbarCommand<TPayload = any> = {
  isToggle?: boolean
  isActive(cmdMgr: CommandManager, ctx: Ctx): boolean
  run(cmdMgr: CommandManager, ctx: Ctx, payload?: TPayload): void
}

