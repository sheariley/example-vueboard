CREATE TABLE user_data.work_item_tag_refs (
  work_item_tag_id INTEGER NOT NULL REFERENCES user_data.work_item_tags (id),
  work_item_id INTEGER NOT NULL REFERENCES user_data.work_items (id) ON DELETE CASCADE,

  PRIMARY KEY (work_item_tag_id, work_item_id)
);

-- Create policies for allowing select and insert
CREATE POLICY "Allow anon/authenticated to read from work_item_tag_refs"
ON "user_data"."work_item_tag_refs"
AS PERMISSIVE
FOR SELECT
TO anon, authenticated
USING (
  EXISTS (
    SELECT 1 FROM user_data.work_items wi
    JOIN user_data.project_columns pc ON wi.project_column_id = pc.id
    JOIN user_data.projects p ON pc.project_id = p.id
    WHERE wi.id = work_item_id
      AND (current_setting('request.jwt.claims', true)::json ->> 'sub') = p.user_id::text
  )
);

CREATE POLICY "Allow anon/authenticated to insert into work_item_tag_refs"
ON "user_data"."work_item_tag_refs"
AS PERMISSIVE
FOR INSERT
TO anon, authenticated
WITH CHECK (
  EXISTS (
    SELECT 1 FROM user_data.work_items wi
    JOIN user_data.project_columns pc ON wi.project_column_id = pc.id
    JOIN user_data.projects p ON pc.project_id = p.id
    WHERE wi.id = work_item_id
      AND (current_setting('request.jwt.claims', true)::json ->> 'sub') = p.user_id::text
  )
);

CREATE POLICY "Allow anon/authenticated to delete from work_item_tag_refs"
ON "user_data"."work_item_tag_refs"
AS PERMISSIVE
FOR DELETE
TO anon, authenticated
USING (
  EXISTS (
    SELECT 1 FROM user_data.work_items wi
    JOIN user_data.project_columns pc ON wi.project_column_id = pc.id
    JOIN user_data.projects p ON pc.project_id = p.id
    WHERE wi.id = work_item_id
      AND (current_setting('request.jwt.claims', true)::json ->> 'sub') = p.user_id::text
  )
);

-- Remove default privileges
REVOKE ALL PRIVILEGES
ON TABLE "user_data"."work_item_tag_refs"
FROM anon, authenticated;

-- Grant select, insert, delete for anon
GRANT SELECT, INSERT, DELETE
ON TABLE "user_data"."work_item_tag_refs"
TO anon, authenticated;

-- Enable RLS
ALTER TABLE "user_data"."work_item_tag_refs" ENABLE ROW LEVEL SECURITY;
