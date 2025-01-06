CREATE OR REPLACE PROCEDURE insert_user_with_authorisation(
    p_username VARCHAR(50),
    p_password_hash VARCHAR(255),
    p_email VARCHAR(255),
    p_role VARCHAR(20)
)
LANGUAGE plpgsql
AS $$
DECLARE
    new_user_id INT;
BEGIN
    -- Start transaction for the procedure
    BEGIN
        -- Insert into users table
        INSERT INTO users (username, created_at)
        VALUES (p_username, NOW())
        RETURNING id INTO new_user_id;

        -- Insert into user_authorisation table
        INSERT INTO user_authorisation (user_id, password_hash, email, role)
        VALUES (new_user_id, p_password_hash, p_email, p_role);

        -- Optional: Output success message
        RAISE NOTICE 'User inserted with ID: %', new_user_id;

    EXCEPTION WHEN OTHERS THEN
        -- Handle errors and rollback
        RAISE EXCEPTION 'Error inserting data: %', SQLERRM;
        ROLLBACK;
    END;
END;
$$;