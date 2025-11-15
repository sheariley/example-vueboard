CREATE TABLE user_data.work_item_tag_ref (
  work_item_tag_id INTEGER NOT NULL REFERENCES user_data.work_item_tag (id),
  work_item_id INTEGER NOT NULL REFERENCES user_data.work_item (id) ON DELETE CASCADE,

  PRIMARY KEY (work_item_tag_id, work_item_id)
);

-- Create policies for allowing select and insert
CREATE POLICY "Allow anon to read from work_item_tag_ref"
ON "user_data"."work_item_tag_ref"
AS PERMISSIVE
FOR SELECT
TO anon
USING (true);

CREATE POLICY "Allow anon to insert into work_item_tag_ref"
ON "user_data"."work_item_tag_ref"
AS PERMISSIVE
FOR INSERT
TO anon
WITH CHECK (true);

-- Remove default privileges
REVOKE ALL PRIVILEGES
ON TABLE "user_data"."work_item_tag_ref"
FROM anon;

-- Grant select and insert for anon
GRANT SELECT, INSERT
ON TABLE "user_data"."work_item_tag_ref"
TO anon;

-- Enable RLS
ALTER TABLE "user_data"."work_item_tag_ref" ENABLE ROW LEVEL SECURITY;
