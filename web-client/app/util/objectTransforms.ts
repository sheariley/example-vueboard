export function createWriteableEntity<T extends object>(entity: T) {
  const result = JSON.parse(JSON.stringify(entity));
  if (typeof result.__typename !== 'undefined') delete result.__typename;
  return result as T;
}