<template>
  <UFieldGroup class="flex *:min-w-9 *:justify-center">
    <UButton
      title="Bold"
      color="neutral"
      variant="subtle"
      @click="toolbarItemClick('bold')"
    >
      <b>B</b>
    </UButton>
    <UButton
      title="Italic"
      color="neutral"
      variant="subtle"
      @click="toolbarItemClick('italic')"
    >
      <i>I</i>
    </UButton>
    <UButton
      title="Strike Through"
      color="neutral"
      variant="subtle"
      @click="toolbarItemClick('strikeThrough')"
    >
      <s>S</s>
    </UButton>
    <UButton
      title="Heading 1"
      color="neutral"
      variant="subtle"
      @click="toolbarItemClick('heading', 1)"
    >
      H1
    </UButton>
    <UButton
      title="Bullet List"
      color="neutral"
      variant="subtle"
      @click="toolbarItemClick('bulletList')"
    >
      <ul>...</ul>
    </UButton>
  </UFieldGroup>
</template>

<script lang="ts" setup>
  import type { CmdKey } from '@milkdown/kit/core'
  import { useInstance as useMilkdownInstance } from '@milkdown/vue'
  import { callCommand } from '@milkdown/kit/utils'
  import {
    toggleEmphasisCommand,
    toggleInlineCodeCommand,
    toggleStrongCommand,
    setBlockTypeCommand
  } from "@milkdown/kit/preset/commonmark";
  import { toggleLinkCommand } from '@milkdown/kit/component/link-tooltip'
  import { toggleStrikethroughCommand } from '@milkdown/kit/preset/gfm'

  const [isMilkdownLoading, getMilkdownInstance] = useMilkdownInstance()

  function callEditorCmd<TKey = unknown>(cmdKey: CmdKey<TKey>, payload?: any) {
    if (isMilkdownLoading.value) return

    const editor = getMilkdownInstance()
    if (!editor) return

    editor.action(callCommand(cmdKey, payload))
  }

  // TODO: Implement "is-active" logic for toolbar buttons (see code on github for crepe's toolbar)

  function toolbarItemClick(commandName: string, payload?: any) {

    // TODO: Finish implementing commands

    switch (commandName) {
      case 'bold':
        callEditorCmd(toggleStrongCommand.key);
        break;
      case 'italic':
        callEditorCmd(toggleEmphasisCommand.key);
        break;
      case 'strikeThrough':
        callEditorCmd(toggleStrikethroughCommand.key);
        break;
      case 'heading':
        // callEditorCmd(toggle, payload);
        break;
      case 'bulletList':
        callEditorCmd(setBlockTypeCommand.key);
        break;
    }
  }
</script>