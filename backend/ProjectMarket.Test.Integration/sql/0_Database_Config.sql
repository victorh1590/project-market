-- Set the search path for the database
ALTER DATABASE postgres SET search_path TO postgres, public;

-- Set the search path for all roles
ALTER ROLE ALL SET search_path TO postgres, public;