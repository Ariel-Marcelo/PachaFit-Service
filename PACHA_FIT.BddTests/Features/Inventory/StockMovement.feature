Feature: StockMovement
    As an inventory manager
    I want to record every movement of my stock
    So that I have a complete and auditable history of my inventory

    Scenario: Register product with initial balance and unit
        When I register a new product "Chía" with SKU "CHIA-001" and initial stock:
            | Quantity | Unit |
            | 500      | g    |
        Then a stock movement should be recorded with the following details:
            | Type    | InputQty | Unit | BaseQtyAffected |
            | Ingreso | 500      | g    | 500             |
        And the movement description should be "Carga inicial de producto"

    Scenario: Register product with expiry date
        When I register a new product "Yogurt" with SKU "YOG-001" expiring on "2026-12-31"
        Then the stock movement should record the expiry date "2026-12-31"

    Scenario: Record manual stock adjustment with unit conversion
        Given the unit of measure "Libra" with abbreviation "lb" has factor 454.0
        And a product exists with SKU "PROT-001"
        When I record a manual adjustment for "PROT-001":
            | Type    | Quantity | Unit | Reason             |
            | Ingreso | 2        | lb   | Ingreso por ajuste |
        Then a stock movement should be recorded with the following details:
            | Type    | InputQty | Unit | BaseQtyAffected |
            | Ingreso | 2        | lb   | 908             |

    Scenario: Register composite product (Kit) assembly
        Given a product exists with Name "Chía" and SKU "CHIA-001"
        And a product exists with Name "Nueces" and SKU "NUE-001"
        When I register a new Kit "Mix Saludable" with SKU "MIX-001" and initial stock 5:
            | BaseProductSku | Quantity | Unit |
            | CHIA-001       | 100      | g    |
            | NUE-001        | 100      | g    |
        Then an "Ingreso" movement should be recorded for "MIX-001" with quantity 5
        And an "Egreso" movement should be recorded for "CHIA-001" with quantity 500
        And an "Egreso" movement should be recorded for "NUE-001" with quantity 500

    Scenario: FEFO Dispatch logic (First Expired, First Out)
        Given a product exists with Name "Arroz" and SKU "ARR-001"
        And the following batches exist for "ARR-001":
            | Quantity | Unit | ExpiryDate |
            | 1        | qq   | 2026-06-01 |
            | 1        | qq   | 2026-12-01 |
        When I dispatch 1 "qq" of "ARR-001"
        Then an "Egreso" movement should be recorded for "ARR-001" with quantity 1
        And the movement should target the batch expiring on "2026-06-01"


