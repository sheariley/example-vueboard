export function createWriteableEntity<T extends object>(entity: T) {
  return JSON.parse(JSON.stringify(entity));
}