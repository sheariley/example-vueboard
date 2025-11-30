CREATE TABLE user_data.project_columns (
  id integer NOT NULL GENERATED ALWAYS AS IDENTITY (INCREMENT 1 START 1 MINVALUE 1 MAXVALUE 2147483647 CACHE 1),
  uid uuid NOT NULL DEFAULT gen_random_uuid(),
  created timestamp DEFAULT CURRENT_TIMESTAMP,
  updated timestamp DEFAULT CURRENT_TIMESTAMP,
  is_deleted boolean DEFAULT false,
  project_id integer REFERENCES user_data.projects (id) ON DELETE CASCADE,

  name varchar(32) NOT NULL,
  fg_color varchar(12) NULL,
  bg_color varchar(12) NULL,
  is_default boolean NOT NULL DEFAULT false,
  index integer NOT NULL DEFAULT 0,

  PRIMARY KEY (id)
);

-- Create policies for allowing select, insert, and update
CREATE POLICY "Allow anon/authenticated to read from project_columns"
ON "user_data"."project_columns"
AS PERMISSIVE
FOR SELECT
TO anon, authenticated
USING (
  EXISTS (
    SELECT 1 FROM user_data.projects p
    WHERE p.id = project_id
      AND (current_setting('request.jwt.claims', true)::json ->> 'sub') = p.user_id::text
  )
);

CREATE POLICY "Allow anon/authenticated to insert into project_columns"
ON "user_data"."project_columns"
AS PERMISSIVE
FOR INSERT
TO anon, authenticated
WITH CHECK (
  EXISTS (
    SELECT 1 FROM user_data.projects p
    WHERE p.id = project_id
      AND (current_setting('request.jwt.claims', true)::json ->> 'sub') = p.user_id::text
  )
);

CREATE POLICY "Allow anon/authenticated to update project_columns"
ON "user_data"."project_columns"
AS PERMISSIVE
FOR UPDATE
TO anon, authenticated
WITH CHECK (
  EXISTS (
    SELECT 1 FROM user_data.projects p
    WHERE p.id = project_id
      AND (current_setting('request.jwt.claims', true)::json ->> 'sub') = p.user_id::text
  )
);

-- Remove default privileges
REVOKE ALL PRIVILEGES
ON TABLE "user_data"."project_columns"
FROM anon, authenticated;

-- Grant select, insert, and update for anon
GRANT SELECT, INSERT, UPDATE
ON TABLE "user_data"."project_columns"
TO anon, authenticated;

-- Enable RLS
ALTER TABLE "user_data"."project_columns" ENABLE ROW LEVEL SECURITY;
