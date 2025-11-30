export function base64EncodeUint8Array(uint8array: Uint8Array) {
  const output = []
  for (let i = 0, length = uint8array.length; i < length; i++)
    output.push(String.fromCharCode(uint8array[i]!))
  return btoa(output.join(''))
}

function asCharCode(c: string) {
  return c.charCodeAt(0)
}

export function base64DecodeUint8Array(chars: string): Uint8Array {
  return Uint8Array.from(atob(chars), asCharCode)
}
