<template>
  <UFieldGroup class="flex *:min-w-9 *:justify-center">
    <UButton
      title="Remove formatting"
      color="neutral"
      variant="subtle"
      @click="toolbarItemClick('clear')"
    >
      <FontAwesomeIcon icon="fa-solid fa-eraser" />
    </UButton>
    <UButton
      title="Bold"
      color="neutral"
      variant="subtle"
      active-variant="solid"
      :active="activeCommands.includes('bold')"
      @click="toolbarItemClick('bold')"
    >
      <FontAwesomeIcon icon="fa-solid fa-bold"/>
    </UButton>
    <UButton
      title="Italic"
      color="neutral"
      variant="subtle"
      active-variant="solid"
      :active="activeCommands.includes('italic')"
      @click="toolbarItemClick('italic')"
    >
      <FontAwesomeIcon icon="fa-solid fa-italic"/>
    </UButton>
    <UButton
      title="Strike Through"
      color="neutral"
      variant="subtle"
      active-variant="solid"
      :active="activeCommands.includes('strikethrough')"
      @click="toolbarItemClick('strikethrough')"
    >
      <FontAwesomeIcon icon="fa-solid fa-strikethrough"/>
    </UButton>
    <UDropdownMenu :items="headingMenuItems">
      <UButton
        title="Heading"
        color="neutral"
        variant="subtle"
        active-variant="solid"
        :active="activeCommands.includes('heading')"
      >
        <FontAwesomeIcon icon="fa-solid fa-heading" width-auto />
        <FontAwesomeIcon icon="fa-solid fa-chevron-down" size="xs" width-auto />
      </UButton>
    </UDropdownMenu>
    <UButton
      title="Block Quote"
      color="neutral"
      variant="subtle"
      active-variant="solid"
      :active="activeCommands.includes('quote')"
      @click="toolbarItemClick('quote')"
    >
      <FontAwesomeIcon icon="fa-solid fa-quote-left"/>
    </UButton>
    <UButton
      title="Link"
      color="neutral"
      variant="subtle"
      active-variant="solid"
      :active="activeCommands.includes('link')"
      @click="toolbarItemClick('link')"
    >
      <FontAwesomeIcon icon="fa-solid fa-link"/>
    </UButton>
    <UButton
      title="Code"
      color="neutral"
      variant="subtle"
      active-variant="solid"
      :active="activeCommands.includes('inline_code')"
      @click="toolbarItemClick('inline_code')"
    >
      <FontAwesomeIcon icon="fa-solid fa-code"/>
    </UButton>
    <UButton
      title="Bullet List"
      color="neutral"
      variant="subtle"
      active-variant="solid"
      :active="activeCommands.includes('bullet_list')"
      @click="toolbarItemClick('bullet_list')"
    >
      <FontAwesomeIcon icon="fa-solid fa-list-ul"/>
    </UButton>
    <UButton
      title="Ordered List"
      color="neutral"
      variant="subtle"
      active-variant="solid"
      :active="activeCommands.includes('ordered_list')"
      @click="toolbarItemClick('ordered_list')"
    >
      <FontAwesomeIcon icon="fa-solid fa-list-ol"/>
    </UButton>
    <UButton
      title="Check List"
      color="neutral"
      variant="subtle"
      active-variant="solid"
      :active="activeCommands.includes('check_list')"
      @click="toolbarItemClick('check_list')"
    >
      <FontAwesomeIcon icon="fa-solid fa-list-check"/>
    </UButton>
  </UFieldGroup>
</template>

<script lang="ts" setup>
  import { useCrepe, type Crepe } from '@milkdown/crepe';
import { commandsCtx, Editor } from '@milkdown/kit/core';
import type { Ctx } from '@milkdown/kit/ctx';
import { useInstance as useMilkdownInstance } from '@milkdown/vue';
import type { DropdownMenuItem } from '@nuxt/ui';
import debounce from 'lodash/debounce';
import { MarkdownToolbarCommands as commands, getActiveToolbarCommandNames, type MarkdownToolbarCommandName } from './toolbar-commands';
import { getNodeAttributeCommand } from './custom-editor-commands';
import { headingSchema } from '@milkdown/kit/preset/commonmark';

  const [isMilkdownLoading, getMilkdownInstance] = useMilkdownInstance()

  const editorRef = ref<Editor>()
  const crepeRef = ref<Crepe>()

  watch(() => isMilkdownLoading.value, (isLoading, wasLoading) => {
    if (!isLoading && wasLoading) {
      editorRef.value = getMilkdownInstance()
      editorRef.value!.action(ctx => {
        crepeRef.value = useCrepe(ctx)

        onEditorLoaded(editorRef.value!, crepeRef.value!)
      })
    }
  }, { immediate: true })

  const activeCommands = ref<MarkdownToolbarCommandName[]>([])
  const activeHeadingLevel = ref<number>()

  const headingMenuItems = computed(() => {
    const activeLevel = activeHeadingLevel.value
    return getHeadingMenuItems(activeLevel)
  })

  const updateActiveCommands = debounce((ctx: Ctx) => {
    const cmdMgr = ctx.get(commandsCtx)
    activeCommands.value = getActiveToolbarCommandNames(cmdMgr, ctx)

    if (!activeCommands.value.includes('heading')) {
      activeHeadingLevel.value = undefined
    } else {
      cmdMgr.call(getNodeAttributeCommand.key, {
        attr: 'level',
        nodeType: headingSchema.type(ctx),
        callback(result) {
          activeHeadingLevel.value = Number(result)
        }
      })
    }
  }, 200)

  function onEditorLoaded(editor: Editor, crepe: Crepe) {
    crepe.on(api => api
      .selectionUpdated(updateActiveCommands)
      .markdownUpdated(updateActiveCommands)
    )
  }

  function toolbarItemClick(commandName: MarkdownToolbarCommandName, payload?: any) {

    // TODO: Finish implementing commands

    if (isMilkdownLoading.value) return

    const editor = editorRef.value
    if (!editor) return

    editor.action(ctx => {
      const cmd = commands[commandName]
      const cmdMgr = ctx.get(commandsCtx)

      cmd.run(cmdMgr, ctx, payload)
    })
  }

  function getHeadingMenuItems(activeLevel: number | null | undefined): DropdownMenuItem[] {
    return Array.from(Array(8).keys())
      .slice(1)
      .map(level => ({
        label: `H${level}`,
        type: 'checkbox',
        color: 'neutral',
        checked: level === activeLevel,
        onSelect() {
          toolbarItemClick('heading', level)
        }
      } as DropdownMenuItem))

  }
</script>