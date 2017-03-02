import Vue from 'vue'
import PricingList from '@/components/PricingList'

describe('PricingList.vue', () => {
  it('should render correct contents', () => {
    const Constructor = Vue.extend(PricingList)
    const vm = new Constructor().$mount()
    expect(vm.$el.textContent)
      .to.contain('Amount')
  })
})
