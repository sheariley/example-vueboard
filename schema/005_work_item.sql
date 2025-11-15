CREATE TABLE user_data.work_item (
  id integer NOT NULL GENERATED ALWAYS AS IDENTITY (INCREMENT 1 START 1 MINVALUE 1 MAXVALUE 2147483647 CACHE 1),
  uid uuid NOT NULL DEFAULT gen_random_uuid(),
  created timestamp DEFAULT CURRENT_TIMESTAMP,
  updated timestamp DEFAULT CURRENT_TIMESTAMP,
  is_deleted boolean DEFAULT false,
  project_column_id integer NOT NULL REFERENCES user_data.project_column (id) ON DELETE CASCADE,

  title varchar(200) NOT NULL,
  description varchar(600) NULL,
  notes text NULL,
  fg_color varchar(12) NULL,
  bg_color varchar(12) NULL,

  PRIMARY KEY (id)
);

-- Create policies for allowing select, insert, and update
CREATE POLICY "Allow anon to read from work_item"
ON "user_data"."work_item"
AS PERMISSIVE
FOR SELECT
TO anon
USING (true);

CREATE POLICY "Allow anon to insert into work_item"
ON "user_data"."work_item"
AS PERMISSIVE
FOR INSERT
TO anon
WITH CHECK (true);

CREATE POLICY "Allow anon to update work_item"
ON "user_data"."work_item"
AS PERMISSIVE
FOR UPDATE
TO anon
WITH CHECK (true);

-- Remove default privileges
REVOKE ALL PRIVILEGES
ON TABLE "user_data"."work_item"
FROM anon;

-- Grant select, insert, and update for anon
GRANT SELECT, INSERT, UPDATE
ON TABLE "user_data"."work_item"
TO anon;

-- Enable RLS
ALTER TABLE "user_data"."work_item" ENABLE ROW LEVEL SECURITY;
