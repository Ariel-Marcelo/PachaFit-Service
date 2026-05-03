Feature: UserService

    Scenario: Get user by ID successfully
        Given a user exists with ID 1 and email "test@example.com"
        When I request the user with ID 1
        Then the result should be successful
        And the user email should be "test@example.com"

    Scenario: User not found when getting by ID
        Given a user with ID 999 does not exist
        When I request the user with ID 999
        Then the result should be a failure
        And the error message should be "Usuario no encontrado para el criterio especificado."

    Scenario: Update user successfully
        Given a user exists with ID 1
        When I update the user with ID 1 with new info:
            | Email               | FullName     |
            | updated@example.com | Updated Name |
        Then the update result should be successful
        And the user update should be persisted

    Scenario: Update non-existent user
        Given a user with ID 999 does not exist
        When I update the user with ID 999 with new info:
            | Email               |
            | updated@example.com |
        Then the update result should be a failure
        And the error message should be "Usuario no encontrado"
