CREATE TABLE user_data.work_items (
  id integer NOT NULL GENERATED ALWAYS AS IDENTITY (INCREMENT 1 START 1 MINVALUE 1 MAXVALUE 2147483647 CACHE 1),
  uid uuid NOT NULL DEFAULT gen_random_uuid(),
  created timestamp DEFAULT CURRENT_TIMESTAMP,
  updated timestamp DEFAULT CURRENT_TIMESTAMP,
  is_deleted boolean DEFAULT false,
  project_column_id integer REFERENCES user_data.project_columns (id) ON DELETE CASCADE,

  title varchar(200) NOT NULL,
  description varchar(600) NULL,
  notes text NULL,
  fg_color varchar(12) NULL,
  bg_color varchar(12) NULL,
  index integer NOT NULL DEFAULT 0,

  PRIMARY KEY (id)
);

-- Create policies for allowing select, insert, and update
CREATE POLICY "Allow anon/authenticated to read from work_items"
ON "user_data"."work_items"
AS PERMISSIVE
FOR SELECT
TO anon, authenticated
USING (
  EXISTS (
    SELECT 1 FROM user_data.project_columns pc
    JOIN user_data.projects p ON pc.project_id = p.id
    WHERE pc.id = project_column_id
      AND (current_setting('request.jwt.claims', true)::json ->> 'sub') = p.user_id::text
  )
);

CREATE POLICY "Allow anon/authenticated to insert into work_items"
ON "user_data"."work_items"
AS PERMISSIVE
FOR INSERT
TO anon, authenticated
WITH CHECK (
  EXISTS (
    SELECT 1 FROM user_data.project_columns pc
    JOIN user_data.projects p ON pc.project_id = p.id
    WHERE pc.id = project_column_id
      AND (current_setting('request.jwt.claims', true)::json ->> 'sub') = p.user_id::text
  )
);

CREATE POLICY "Allow anon/authenticated to update work_items"
ON "user_data"."work_items"
AS PERMISSIVE
FOR UPDATE
TO anon, authenticated
WITH CHECK (
  EXISTS (
    SELECT 1 FROM user_data.project_columns pc
    JOIN user_data.projects p ON pc.project_id = p.id
    WHERE pc.id = project_column_id
      AND (current_setting('request.jwt.claims', true)::json ->> 'sub') = p.user_id::text
  )
);

-- Remove default privileges
REVOKE ALL PRIVILEGES
ON TABLE "user_data"."work_items"
FROM anon, authenticated;

-- Grant select, insert, and update for anon
GRANT SELECT, INSERT, UPDATE
ON TABLE "user_data"."work_items"
TO anon, authenticated;

-- Enable RLS
ALTER TABLE "user_data"."work_items" ENABLE ROW LEVEL SECURITY;
