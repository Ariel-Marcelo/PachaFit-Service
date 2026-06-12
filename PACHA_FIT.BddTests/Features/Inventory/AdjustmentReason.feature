Feature: AdjustmentReason
    As an inventory manager
    I want to use predefined adjustment reasons
    So that I can have clean and consistent reports

    Scenario Outline: Verify predefined adjustment reasons
        When I check if "<Reason>" is a valid predefined reason
        Then the result should be valid

        Examples:
            | Reason              |
            | Caducidad           |
            | Rotura              |
            | Consumo Interno     |
            | Error de Inventario | 

    Scenario: Reject an invalid adjustment reason
        When I check if "Cualquier otra cosa" is a valid predefined reason
        Then the result should be invalid

    Scenario: List all predefined adjustment reasons
        When I request all predefined adjustment reasons
        Then the list should contain exactly:
            | Name                |
            | Caducidad           |
            | Rotura              |
            | Consumo Interno     |
            | Error de Inventario |
