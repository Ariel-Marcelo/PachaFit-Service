Feature: Product Management
    As an inventory manager
    I want to manage products with their specifications
    So that I can keep track of my inventory and product details

    Scenario: Create a new product with zero initial stock
        When I create a new product with the following details:
            | Name    | SKU      | CategoryId | CostPrice | SalePrice | IvaPercentage | IsWeightBased |
            | Protein | PROT-001 | 1          | 25.50     | 45.00     | 15            | false         |
        Then the product should be created successfully
        And the initial stock should be 0
        And the IVA percentage should be 15
        And the product should not be weight-based

    Scenario: Create a weight-based product with specifications
        Given the following specifications:
            | Label  | Value   |
            | Origin | Ecuador |
        When I create a new product with the following details:
            | Name   | SKU      | IvaPercentage | IsWeightBased |
            | Cheese | CHS-001  | 0             | true          |
        Then the product should be created successfully
        And the IVA percentage should be 0
        And the product should be weight-based
        And the specifications should include:
            | Label  | Value   |
            | Origin | Ecuador |

    Scenario: Create a composite product (Kit)
        Given a product exists with Name "Chía" and SKU "CHIA-001"
        And a product exists with Name "Nueces" and SKU "NUE-001"
        And the following composition:
            | BaseProductSku | Quantity | UnitAbbreviation |
            | CHIA-001       | 100      | g                |
            | NUE-001        | 100      | g                |
        When I create a new product with the following details:
            | Name               | SKU      | SalePrice |
            | Kit Mix Saludable  | KIT-001  | 10.00     |
        Then the product should be created successfully
        And the product composition should include:
            | BaseProductName | Quantity | UnitAbbreviation |
            | Chía            | 100      | g                |
            | Nueces          | 100      | g                |
