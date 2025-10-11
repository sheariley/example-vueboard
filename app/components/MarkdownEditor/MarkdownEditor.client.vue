<template>
  <div class="flex flex-col items-stretch gap-1">
    <MarkdownEditorToolbar />
    <Milkdown />
  </div>
</template>

<script lang="ts" setup>
  import { type CmdKey } from "@milkdown/kit/core";
  import { callCommand, replaceAll } from '@milkdown/kit/utils'
  import { history as milkdownHistory } from '@milkdown/kit/plugin/history';
  import { useEditor, Milkdown } from '@milkdown/vue';
  import { Crepe } from '@milkdown/crepe';
  
  const modelValue = defineModel<string | null>({
    required: true
  })

  let modelValueSync = false

  let crepe: Crepe;
  
  const { loading: milkdownLoading } = useEditor((root) =>{
    crepe = new Crepe({
      root,
      features: {
        [Crepe.Feature.BlockEdit]: false,
        [Crepe.Feature.Latex]: false,
        [Crepe.Feature.ImageBlock]: false,
        [Crepe.Feature.Toolbar]: false,
        [Crepe.Feature.ListItem]: true,
        [Crepe.Feature.LinkTooltip]: true,
        [Crepe.Feature.CodeMirror]: true,
        [Crepe.Feature.Table]: true
      },
      defaultValue: modelValue.value || ''
    })

    crepe.editor.use(milkdownHistory)

    crepe.on(api => {
      api.markdownUpdated((ctx, markdown, prevMarkdown) => {
        if (markdown === prevMarkdown) return

        modelValueSync = true

        if (!markdown?.length)
          modelValue.value = null
        else
          modelValue.value = markdown

        queueMicrotask(() => {
          modelValueSync = false
        })
      })
    })

    return crepe;
  });

  function callEditorCmd<TKey = unknown>(cmdKey: CmdKey<TKey>, payload?: any) {
    if (milkdownLoading.value) return

    crepe.editor.action(callCommand(cmdKey, payload))
  }

  watch(() => modelValue.value, value => {
    if (!modelValueSync && crepe?.editor) {
      crepe.editor.action(replaceAll(value || '', false))
    }
  })

  onUnmounted(() => {
    if (crepe) {
      crepe.destroy()
    }
  })

</script>
