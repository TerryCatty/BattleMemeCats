mergeInto(LibraryManager.library, {

	SecondChanceExtern: function(){
		ysdk.adv.showRewardedVideo({
			callbacks:{
				onOpen: () => {
					console.log("Video ad open");
				},
				onRewarded: () => {
					console.log('Rewarded!');
					MyGameInstance.SendMessage('GameManager','AdvIsShowSecondChance');
				},
				onClose: () => {
					console.log('Video ad closed.');
				}, 
				onError: (e) => {
					console.log('Error while open video ad:', e);
				}
			}
		})
	},

	HealTowerExtern: function(){
		ysdk.adv.showRewardedVideo({
			callbacks:{
				onOpen: () => {
					console.log("Video ad open");
				},
				onRewarded: () => {
					console.log('Rewarded!');
					MyGameInstance.SendMessage('GameManager','AdvIsShowHeal');
				},
				onClose: () => {
					console.log('Video ad closed.');
				}, 
				onError: (e) => {
					console.log('Error while open video ad:', e);
				}
			}
		})
	},

	ShowAdvExtern: function(){
		ysdk.adv.showFullscreenAdv({
			callbacks: {
				onClose: function(wasShown) {
          // some action after close
				},
				onError: function(error) {
          // some action on error
				}
			}
		})
	},

	SaveExtern: function(date){
		var dateString = UTF8ToString(date);
		var myObj = JSON.parse(dateString);
		player.setData(myObj);
	},

	LoadExtern: function(){
		player.getData().then(_date =>{
			const myJSON = JSON.stringify(_date);
			MyGameInstance.SendMessage("GameManager", "LoadData", myJSON);
		});
	},

	
	/*ShowStickyBanner: function(){
		ysdk.adv.showBannerAdv();
	},
	HideStickyBanner: function(){
		ysdk.adv.hideBannerAdv();
	}*/
});