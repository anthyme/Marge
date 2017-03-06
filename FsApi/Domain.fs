module Domain

open System

type Event =
| PriceCreated of cost : decimal * price : decimal * discount : decimal * profit : decimal
| DiscountChanged of price : decimal * discount : decimal * profit : decimal
| PriceDeleted

type WrapedEvent = EventWrapper of Guid * Event

module Command =
    type Commands =
    | CreatePrice of targetPrice : decimal * cost : decimal
    | ChangeDiscount of discount : int
    | DeleteCommand

    type Price = {  Cost: decimal ; TargetPrice: decimal }

    let decide command (state:Price) =
        let computeProfit price cost = 1m - cost / price
        match command with
        | CreatePrice (targetPrice, cost) -> 
            [PriceCreated (cost=cost, price=targetPrice, discount=0m, profit=computeProfit targetPrice cost)]
        | ChangeDiscount (discount) ->
            let price = state.TargetPrice - (state.TargetPrice * 0.01m * decimal discount)
            [DiscountChanged (price=price, discount=decimal discount, profit=computeProfit price state.Cost)]
        | _ -> [PriceDeleted]
    
    let apply state event =
        match event with
        | PriceCreated (cost,price,_,_) -> { Cost = cost ; TargetPrice = price }
        | _ -> state

module Query =
    open FSharp.Data
    open FSharp.Data.Sql

    let [<Literal>] db = @"Data Source=(LocalDb)\MSSQLLocalDB;AttachDbFilename=C:\db\marge.mdf;Initial Catalog=Marge;Integrated Security=True"

    module Queries =
        type sql = SqlDataProvider<ConnectionString=db, UseOptionTypes=true>
        let database = sql.GetDataContext()

        //let getById id =  use cmd = new SqlCommandProvider<"select * from Prices where id = @id", db>(db) in cmd.Execute id
        let getById id =  query { for price in database.Dbo.Prices do
                                  where (price.Id = id)
                                  select price } |> Seq.tryHead

        //let getAll _ = use cmd = new SqlCommandProvider<"select * from Prices", db>(db) in cmd.Execute()
        let getAll _ = database.Dbo.Prices |> List.ofSeq

    [<AutoOpen>]
    module private SideEffects =
        let create (amount, discount, profit, id) = 
            use cmd = new SqlCommandProvider<"insert Prices(Amount, Discount, Profit, Id) values (@Amount, @Discount, @Profit, @Id)", db>(db)
            cmd.Execute  (amount, discount, profit, id)

        let update price =
            use cmd = new SqlCommandProvider<"update Prices set Amount = @Amount, Discount = @Discount, Profit = @Profit where id = @id", db>(db)
            cmd.Execute price
        
        let delete id = use cmd = new SqlCommandProvider<"delete from Prices where id = @id", db>(db) in cmd.Execute id

    let handleEvent (id,event) =
        match event with
        | PriceCreated(cost, price, discount, profit) -> create (price, discount, profit, id)
        | DiscountChanged(price, discount, profit) -> update (price, discount, profit, id)
        | PriceDeleted -> delete id
