Feature: AuthService

    Scenario: Login user successfully
        Given a user exists with email "test@example.com" and password "Password123"
        When I login with email "test@example.com" and password "Password123"
        Then the login result should be successful
        And the session should have email "test@example.com"

    Scenario: Login user with wrong password
        Given a user exists with email "test@example.com" and password "Password123"
        When I login with email "test@example.com" and password "wrongpassword"
        Then the login result should be a failure
        And the login error message should be "Credenciales incorrectas"

    Scenario: Login non-existent user
        Given a user with email "nonexistent@example.com" does not exist
        When I login with email "nonexistent@example.com" and password "Password123"
        Then the login result should be a failure
        And the login error message should be "Usuario no encontrado"

    Scenario: Sign up user successfully
        Given a user with email "newuser@example.com" does not exist
        When I sign up with:
            | Email               | Password    | FullName    |
            | newuser@example.com | Password123 | New User    |
        Then the sign up result should be successful
        And the user should be saved

    Scenario: Sign up user that already exists
        Given a user exists with email "existing@example.com"
        When I sign up with:
            | Email                | Password    | FullName    |
            | existing@example.com | Password123 | Existing    |
        Then the sign up result should be a failure
        And the sign up error message should be "El usuario ya existe"
