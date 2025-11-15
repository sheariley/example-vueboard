CREATE TABLE user_data.work_item_tag (
  id integer NOT NULL GENERATED ALWAYS AS IDENTITY (INCREMENT 1 START 1 MINVALUE 1 MAXVALUE 2147483647 CACHE 1),
  uid uuid NOT NULL DEFAULT gen_random_uuid(),
  user_id uuid NOT NULL REFERENCES auth.users (id),

  tag_text varchar(30) NOT NULL,

  PRIMARY KEY (id)
);

-- Create policies for allowing select, insert, and update
CREATE POLICY "Allow anon to read from work_item_tag"
ON "user_data"."work_item_tag"
AS PERMISSIVE
FOR SELECT
TO anon
USING (true);

CREATE POLICY "Allow anon to insert into work_item_tag"
ON "user_data"."work_item_tag"
AS PERMISSIVE
FOR INSERT
TO anon
WITH CHECK (true);

-- Remove default privileges
REVOKE ALL PRIVILEGES
ON TABLE "user_data"."work_item_tag"
FROM anon;

-- Grant select, insert, and update for anon
GRANT SELECT, INSERT
ON TABLE "user_data"."work_item_tag"
TO anon;

-- Enable RLS
ALTER TABLE "user_data"."work_item_tag" ENABLE ROW LEVEL SECURITY;
